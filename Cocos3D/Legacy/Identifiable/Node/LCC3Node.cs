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
using System.Collections.Generic;

namespace Cocos3D
{
    public class LCC3Node
    {
        // Instance fields
       
        LCC3Node _parent;
        List<LCC3Node> _children;

        LCC3Matrix4x4 _transformMatrix;
        LCC3Vector _location;
        LCC3Vector _globalLocation;
        bool _isTransformDirty;

        #region Properties

        public LCC3Node Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public virtual bool ShouldUseLighting
        {
            get 
            { 
                foreach (LCC3Node child in _children)
                {
                    if (child.ShouldUseLighting == true) return true;
                }

                return false;
            }
        }

        public virtual LCC3Scene Scene
        {
            get { return _parent.Scene; }
        }

        public LCC3Matrix4x4 TransformMatrix
        {
            get { return _transformMatrix; }
            set
            {
                this.UpdateGlobalOrientation();
                //this.TransformMatrixChanged();
                //this.NotifyTransformListeners();
            }
        }

        public LCC3Vector Location
        {
            get { return _location; }
            set { _location = value; this.MarkTransformDirty(); }
        }

        public LCC3Vector GlobalLocation
        {
            get { return _globalLocation; }
        }

        public virtual LCC3Vector4 GlobalHomogeneousPosition
        {
            get { return new LCC3Vector4(this.GlobalLocation, 1.0f); }
        }

        #endregion Properties

        public LCC3Node()
        {
            _transformMatrix = LCC3Matrix4x4.CC3MatrixIdentity;
        }

        private void MarkTransformDirty()
        {
            _isTransformDirty = true;
        }

        private void UpdateGlobalOrientation()
        {
            this.UpdateGlobalLocation();
            //this.UpdateGlobalRotation();
            //this.UpdateGlobalScale();
        }

        private void UpdateGlobalLocation()
        {
            LCC3Vector4 locVec4 = new LCC3Vector4(_location, 0.0f);
            _globalLocation = _transformMatrix.TransformCC3Vector4(locVec4).TruncateToCC3Vector();
        }

        public void ApplyLocalTransforms()
        {
            this.ApplyTranslation();
            //this.ApplyRotation();
            //this.ApplyScaling();
        }

        private void ApplyTranslation()
        {
            _transformMatrix *= LCC3Matrix4x4.CreateTranslationMatrix(_location);
            this.UpdateGlobalLocation();
        }
    }
}

