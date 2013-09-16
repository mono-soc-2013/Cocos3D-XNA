//
// Copyright 2013 Rami Tabbara
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
//
// Please see README.md to locate the external API documentation.
//
using System;
using Microsoft.Xna.Framework.Graphics;
using Cocos2D;

namespace Cocos3D
{
    public struct LCC3Viewport : IEquatable<LCC3Viewport>
    {
        // ivars

        Viewport _xnaViewport;

        #region Properties

        // Public instance properties

        public int X
        {
            get { return _xnaViewport.X; }
        }

        public int Y
        {
            get { return _xnaViewport.Y; }
        }

        public int Width
        {
            get { return _xnaViewport.Width; }
        }

        public int Height
        {
            get { return _xnaViewport.Height; }
        }

        // Internal instance properties

        internal Viewport XnaViewport
        {
            get { return _xnaViewport; }
        }

        #endregion Properties


        #region  Operators

        public static bool operator ==(LCC3Viewport value1, LCC3Viewport value2)
        {
            return (value1.X == value2.X) && (value1.Y == value2.Y) 
                && (value1.Width == value2.Width) && (value1.Height == value2.Height);
        }

        public static bool operator !=(LCC3Viewport value1, LCC3Viewport value2)
        {
            return (value1.X != value2.X) || (value1.Y != value2.Y) 
                || (value1.Width != value2.Width) || (value1.Height != value2.Height);
        }

        #endregion Operators


        #region Constructors

        public LCC3Viewport(int x, int y, int width, int height)
        {
            _xnaViewport = new Viewport(x, y, width, height);
        }

        public LCC3Viewport(LCC3Viewport vp, int xOffset, int yOffset, int widthOffset, int heightOffset)
        : this(vp.X + xOffset, vp.Y + yOffset, vp.Width + widthOffset, vp.Height + heightOffset)
        {

        }

        public LCC3Viewport(CCRect rect)
        : this((int)rect.Origin.X, (int)rect.Origin.Y, (int)rect.Size.Width, (int)rect.Size.Height)
        {

        }

        internal LCC3Viewport(Viewport xnaViewport)
        : this(xnaViewport.X, xnaViewport.Y, xnaViewport.Width, xnaViewport.Height)
        {

        }

        #endregion Constructors


        #region Instance methods

        // Equality handling

        public override bool Equals(object obj)
        {
            // Using overloaded == operator
            return (obj is LCC3Viewport) ? this == (LCC3Viewport)obj : false;
        }

        public bool Equals(LCC3Viewport other)
        {
            // Using overloaded == operator
            return this == other;
        }

        public override int GetHashCode()
        {
            return (int)(this.X + this.Y + this.Width + this.Height);
        }

        // Public viewport calculation instance methods

        public bool ContainsPoint(CCPoint point)
        {
            return (point.X >= this.X) && (point.X <= this.X + this.Width) 
                && (point.Y >= this.Y) && (point.Y <= this.Y + this.Height);
        }

        // Other public instance methods

        public CCRect ToCCRect()
        {
            return new CCRect(this.X, this.Y, this.Width, this.Height);
        }

        #endregion Instance methods
    }
}

