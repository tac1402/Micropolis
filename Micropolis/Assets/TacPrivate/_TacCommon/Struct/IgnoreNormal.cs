// Author: Sergej Jakovlev <tac1402@gmail.com>
// Copyright (C) 2024 Sergej Jakovlev
// You can use this code for educational purposes only;
// this code or its modifications cannot be used for commercial purposes
// or in proprietary libraries without permission from the author

using System;

namespace TAC
{

    [Serializable]
    public class IgnoreNormal
    {
        public bool X = true;
        public bool Y = true;
        public bool Z = true;

        public IgnoreNormal() { }
        public IgnoreNormal(bool argX, bool argY, bool argZ)
        {
            X = argX;
            Y = argY;
            Z = argZ;
        }

        public IgnoreNormal GetReverse()
        {
            bool x = (X == true ? false : true);
            bool z = (Z == true ? false : true);
            return new IgnoreNormal(x, Y, z);
        }
    }
}