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
        public new static string AlgorithmName => "Experimental";


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

        class BoardEx : Board
        {
            public BoardEx()
            {

            }

            public BoardEx(Board board)
            {
                this.Name = board.Name;
                this.Material = board.Material;
                this.Length = board.Length;
                this.Width = board.Width;
                this.Orig = board;
                this.OffsetLength = board.OffsetLength;
                this.OffsetWidth = board.OffsetWidth;
            }

            public Board Orig { get; set; }
            public BoardEx Peer { get; set; }

            public override string ToString()
            {
                return $"{Name} [{Length,7:0.0} x {Width,5:0.0}] @ [{OffsetLength,7:0.0} ; {OffsetWidth,5:0.0}]";
            }
        }

        public override void Pack(Part[] parts, Board[] boards, double sawkerf = 3.2, double partLengthPadding = 0, double partWidthPadding = 0)
        {
            // Sort the boards asc on area
            var sortedBoards = boards.OrderBy(b => b.Area).Select(t=>new BoardEx(t) ).ToList();

            Dictionary<Part, List<BoardEx>> potentialSources = new Dictionary<Part, List<BoardEx>>();
            parts.OrderByDescending(o => o.Area).ToList().ForEach(p => potentialSources.Add(p, null));

            do
            {
                // if there are no more parts we can place, exit.
                if (potentialSources.Count() == 0) break;

                // find all boards that each part will fit on
                List<Part> unpackedParts = potentialSources.Keys.ToList();
                unpackedParts.ForEach(p => potentialSources[p] = sortedBoards.Where(b => b.Length > p.Length && b.Width > p.Width).ToList());

                // sort the parts-sources according to count of the sources
                var sortedSources = potentialSources.OrderBy(s => s.Value.Count()).ToList();

                var pairToProcess = sortedSources[0];
                var partToPlace = pairToProcess.Key;
                var potSources = pairToProcess.Value;

                // if the first has 0 sources, it means we cannot pack it, so remove it from the list, and continue to the next
                if (potSources == null || potSources.Count() == 0)
                {
                    potentialSources.Remove(partToPlace);
                    continue;
                }

                // place the first part on the first source
                var BoardToUse = potSources[0];
                partToPlace.Source = BoardToUse.Orig;
                partToPlace.OffsetLength = BoardToUse.OffsetLength;
                partToPlace.OffsetWidth = BoardToUse.OffsetWidth;

                // remove the part placed from the list of parts to place
                potentialSources.Remove(partToPlace);

                // remove the board from the list of avail boards
                sortedBoards.Remove(BoardToUse);
                // adjust the peer board
                if (BoardToUse.Peer != null)
                {
                    if (BoardToUse.Name == "T")
                    {
                        if (BoardToUse.OffsetWidth + partToPlace.Width > BoardToUse.Peer.OffsetWidth)
                            BoardToUse.Peer.Length = BoardToUse.OffsetLength - BoardToUse.Peer.OffsetLength;
                        else
                            BoardToUse.Width = BoardToUse.Peer.OffsetWidth - BoardToUse.OffsetWidth;
                    }
                    else
                    {
                        if (BoardToUse.OffsetLength + partToPlace.Length > BoardToUse.Peer.OffsetLength)
                            BoardToUse.Peer.Width = BoardToUse.OffsetWidth - BoardToUse.Peer.OffsetWidth;
                        else
                            BoardToUse.Length = BoardToUse.Peer.OffsetLength - BoardToUse.OffsetLength;
                    }
                    // unlink the peer
                    BoardToUse.Peer.Peer = null;
                }

                // add back the remainder of the board used
                BoardEx L1 = new BoardEx
                {
                    Orig = BoardToUse.Orig,
                    Name = "T",
                    Length = BoardToUse.Length - partToPlace.Length - sawkerf,
                    Width = BoardToUse.Width,
                    OffsetLength = BoardToUse.OffsetLength + partToPlace.Length + sawkerf,
                    OffsetWidth = BoardToUse.OffsetWidth
                };
                BoardEx L2 = new BoardEx
                {
                    Orig = BoardToUse.Orig,
                    Name = "H",
                    Length = BoardToUse.Length,
                    Width = BoardToUse.Width - partToPlace.Width - sawkerf,
                    OffsetLength = BoardToUse.OffsetLength,
                    OffsetWidth = BoardToUse.OffsetWidth + partToPlace.Width + sawkerf,
                    Peer = L1
                };
                L1.Peer = L2;

                //if (L1.Width > 4 && L1.Length > 4)
                sortedBoards.Add(L1);

                //if (L2.Length>4 && L2.Width>4)
                sortedBoards.Add(L2);

                // resort the boards
                sortedBoards = sortedBoards.OrderBy(b => b.Area).ToList();

            } while (true);
        }
    }
}
