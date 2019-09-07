using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker.Packers
{
    class experimental : PackerBase
    {
        new public static string AlgorithmName => "Experimental";


        StockItem[] GetStockItemsFitting(Part part, StockItem[] boards)
        {
            List<StockItem> tmp = new List<StockItem>();
            for (int i = 0; i < boards.Length; i++)
            {
                StockItem iBoard = boards[i];

                if (iBoard.Length < part.Length) continue;
                if (iBoard.Width < part.Width) continue;
                tmp.Add(iBoard);
            }

            return tmp.ToArray();
        }

        class PartWithSources
        {
            public Part part { get; set; }

            public StockItem[] Sources { get; set; }

        }

#if drawdbgimages
        private Bitmap Drawboard_debug(StockItemWithParent board, StockItemWithParent[] freerects, int len)
        {
            double xMargin = 50;
            double yMargin = 50;

            double imageHeight = board.Board.Width + 2 * yMargin;
            double imageWidth = board.Board.Length + 2 * xMargin;

            // create bitmap
            Bitmap bitmap = new Bitmap((int)imageWidth, (int)imageHeight);
            Graphics g = Graphics.FromImage(bitmap);
            // draw the board
            g.DrawRectangle(Pens.Black, (float)xMargin, (float)yMargin, (float)board.Board.Length, (float)board.Board.Width);

            //draw the board segments
            for (int i = 0; i < len; i++)
            {
                RectangleF tF = new RectangleF((float)freerects[i].dLength,(float)freerects[i].dWidth,(float)freerects[i].Board.Length,(float)freerects[i].Board.Width);
                Rectangle t = Rectangle.Round(tF); 
                t.Offset((int)xMargin, (int)yMargin);
                g.FillRectangle(new SolidBrush(Color.FromArgb(120,Color.Red)), t);
            }

            // draw the parts placed
            for (int i = 0; i < board.Board.PackedPartsCount; i++)
            {
                Placement iPlacement = board.Board.PackedParts[i];

                // draw the part
                g.FillRectangle(Brushes.Green, (float)(xMargin + iPlacement.dLength), (float)(yMargin + iPlacement.dWidth), (float)iPlacement.Part.Length, (float)iPlacement.Part.Width);

                // print the part text
                string partLabel = $"{iPlacement.Part.Name}";
                Font partFont = new Font(new FontFamily("Microsoft Sans Serif"), 10);
                g.DrawString(partLabel, partFont, Brushes.Black, (float)(xMargin + iPlacement.dLength), (float)(yMargin + iPlacement.dWidth));
            }
            // draw the board
            //g.DrawRectangle(Pens.Black, (float)xMargin, (float)yMargin, (float)board.Length, (float)board.Width);
            Font aFont = new Font(new FontFamily("Microsoft Sans Serif"), 10);
            g.DrawString((board.Board.PackingCoverage).ToString("0.00%"), aFont, Brushes.Black, (float)(xMargin), (float)(bitmap.Height - yMargin));

            g.Flush();
            return bitmap;
        }
#endif

        public override void Pack(Part[] parts, StockItem[] boards, double sawkerf = 3.2, double partLengthPadding = 0, double partWidthPadding = 0)
        {
            #region MyRegion




            //sort the boards according to area (ASC)
            //for each available board, 
            //  for each unpacked part
            //      if this board will accomodate this part    
            //          append this board to the list of boards that will acommodate the part

            //placepart:
            //sort the list of unpacked parts on # accomodating boards (ASC)
            //possibleflag = false
            //loop through the accommodating boards for the first part
            //   possibleflag = true
            //   remove the part from the list of unplaced parts
            //   remove the used board from all other parts' lists of accomodating boards
            //   remove the used board from the list of available boards
            //   for each of the remainder sub-boards of the board used
            //       add the sub-board to the list of available boards, preserving sort order
            //       for each uplaced part
            //           if the remainder will accomodate the part
            //               append this sub-board to the list of boards that will acommodate the part
            //
            //   recursedpossibleflag = placepart(rest of parts, available boards)
            //   if recursedpossibleflag
            //      save solution
            //endloop
            //return possibleflag

            #endregion

            var x = test1(parts.Where(t => Convert.ToInt16(t.Name) < 9).ToArray(), 
                boards.OrderBy(t => t.Area).ToArray(), 
                sawkerf, 
                partLengthPadding, 
                partWidthPadding);

            foreach (var iboard in boards)
                iboard.PackedParts = new Placement[0];
            
            foreach (var item in x)
            {
                var iboard = boards.First(t => t.Name == item.Sources[0].Name);
                var lst = iboard.PackedParts.ToList();
                lst.Add(
                    new Placement()
                    {
                        dLength = 0,
                        dWidth = 0,
                        Part = item.part
                    }
                    );
                iboard.PackedParts = lst.ToArray();
            }


        }


        //sort the boards according to area (ASC)
        //per part, list accomodating boards

        //while count of unplaced parts > 0
        //  sort the list of unpacked parts on # accomodating boards (ASC)
        //  if the first part has 0 accomodating boards
        //      exit with "no solution"
        //
        //  place the first part on first board that will accomodate it - this would be the smallest...
        //  remove the part from the list of unplaced parts
        //  remove the used board from all other parts' lists of accomodating boards
        //  remove the used board from the list of available boards
        //  for each of the remainder sub-boards of the board used
        //      add the sub-board to the list of available boards, preserving sort order
        //      for each uplaced part
        //          if the remainder will accomodate the part
        //              append this sub-board to the list of boards that will acommodate the part
        //  

        //two possible returns:
        //  null : no solution down this branch
        //  [] : solution reached for this branch

        private PartWithSources[] test1(Part[] parts, StockItem[] boards, double sawkerf = 3.2, double partLengthPadding = 0, double partWidthPadding = 0)
        {
            // convert the parts to the algorithm's extended version and check which boards will accommodate the parts
            PartWithSources[] partsEx = parts.Select(t =>
                new PartWithSources()
                {
                    part = t,
                    Sources = boards.Where(q => q.Length > t.Length && q.Width > t.Width).ToArray()
                }
                ).OrderBy(o => o.Sources.Length).ToArray();

            PartWithSources partToCut = partsEx[0];
            
            // if a part can not be placed, return null indicating there is no solution down this branch
            if (partToCut.Sources.Length == 0) return null;

            StockItem boardToCut = partToCut.Sources[0];

            // place the part on the board
            if (partToCut.Sources.Length > 1) partToCut.Sources = new StockItem[] { boardToCut };

            // remove the part from the list of uncut parts
            var remainingParts = parts.Where(t => t != partToCut.part).ToArray();
            if (remainingParts.Length == 0) return new PartWithSources[] { partToCut };

            // remove the board used from the list of available boards
            var remainingBoards = boards.Where(t => t != boardToCut).ToList();
            // add back the remainder 2 boards of the cut board
            remainingBoards.Add
                (new StockItem { Name = boardToCut.Name, Material = boardToCut.Material, Width = boardToCut.Width, Length = boardToCut.Length - partToCut.part.Length });
            remainingBoards.Add
                (new StockItem { Name = boardToCut.Name, Material = boardToCut.Material, Width = boardToCut.Width - partToCut.part.Width, Length = partToCut.part.Length });


            // pack the rest of the parts
            var remainderSolution = test1(remainingParts, remainingBoards.OrderBy(t=>t.Area).ToArray(), sawkerf, partLengthPadding, partWidthPadding);
            if (remainderSolution == null) return null;

            var lst = remainderSolution.ToList();
            lst.Add(partToCut);
            return lst.ToArray();
        }



    }
}
