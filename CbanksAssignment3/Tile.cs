using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CbanksAssignment3
{
    public class Tile : Button
    {
        private bool isEmptySpace = false;
        private int startingIndex = 0;
        private int destinationIndex = 0;
        private int currentIndex = 0;
        public bool IsEmptySpace
        {
            get
            {
                return isEmptySpace;
            }

            set
            {
                isEmptySpace = value;
            }
        }

        public int StartingIndex
        {
            get
            {
                return startingIndex;
            }

            set
            {
                startingIndex = value;
            }
        }

        public int DestinationIndex
        {
            get
            {
                return destinationIndex;
            }

            set
            {
                destinationIndex = value;
            }
        }

        public int CurrentIndex
        {
            get
            {
                return currentIndex;
            }

            set
            {
                currentIndex = value;
            }
        }
    }

    
}

