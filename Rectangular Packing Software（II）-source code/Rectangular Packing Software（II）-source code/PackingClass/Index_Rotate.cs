using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackingClass
{
    public class Index_Rotate
    {
        private int index;
        private int rotate;

        public Index_Rotate()
        {

        }

        public Index_Rotate(int _index, int _rotate)
        {
            this.index = _index;
            this.rotate = _rotate;
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public int Rotate
        {
            get { return rotate; }
            set { rotate = value; }
        }

    }
}
