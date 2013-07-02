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
    public abstract class CC3DrawableNode : CC3Node
    {
        // Instance fields

        protected CC3GraphicsContext _graphicsContext;
        protected List<CC3DrawableNode> _drawableNodeChildren;

        private CC3Vector _worldScale;
        private CC3Quaternion _rotationChangeRelativeToAnchorNeededToUpdate;
        private CC3Vector _rotationAnchorPointRelativeToPositionUsedForUpdate;

        #region Properties

        // Instance properties

        public virtual CC3Quaternion LocalRotation
        {
            get { return _worldMatrix.LocalRotationOfTransformMatrix(); }
            set
            {
                CC3Quaternion rotationChange = this.LocalRotation.Inverse() * value;
                this.IncrementallyUpdateWorldTransform(CC3Vector.CC3VectorZero,
                                                       CC3Vector.CC3VectorZero,
                                                       rotationChange,
                                                       CC3Vector.CC3VectorZero);
            }
        }

        public virtual CC3Vector WorldScale
        {
            get { return _worldScale; }
            set
            {
                CC3Vector scaleChange = value - _worldScale;
                this.IncrementallyUpdateWorldTransform(CC3Vector.CC3VectorZero, 
                                                       scaleChange,
                                                       CC3Quaternion.CC3QuaternionZero,
                                                       CC3Vector.CC3VectorZero);
            }
        }

        #endregion Properties


        #region Constructors

        public CC3DrawableNode(CC3GraphicsContext graphicsContext) : base()
        {
            _graphicsContext = graphicsContext;
            _drawableNodeChildren = new List<CC3DrawableNode>();

            _worldScale = CC3Vector.CC3VectorUnitCube;
            _rotationChangeRelativeToAnchorNeededToUpdate = CC3Quaternion.CC3QuaternionIdentity;
            _rotationAnchorPointRelativeToPositionUsedForUpdate = CC3Vector.CC3VectorZero;
        }

        #endregion Constructors


        #region Updating world matrix methods

        internal void IncrementallyUpdateWorldTransform(CC3Vector translationChange, 
                                                        CC3Vector scaleChange, 
                                                        CC3Quaternion rotationChangeRelativeToAnchor,
                                                        CC3Vector rotationAnchorPointRelativeToPosition)
        {
            this.WorldTranslationChangeNeededToUpdate = translationChange;
            _rotationChangeRelativeToAnchorNeededToUpdate = rotationChangeRelativeToAnchor;
            _rotationAnchorPointRelativeToPositionUsedForUpdate = rotationAnchorPointRelativeToPosition;

            _worldScale += scaleChange;

            this.ShouldUpdateWorldMatrix();

            foreach (ICC3NodeTransformObserver transformObserver in _listOfNodeTransformObservers)
            {
                transformObserver.ObservedNodeWorldTransformDidChange(this, 
                                                                      translationChange, 
                                                                      scaleChange, 
                                                                      rotationChangeRelativeToAnchor,
                                                                      rotationAnchorPointRelativeToPosition);
            }
        }

        protected override void UpdateWorldMatrix()
        {
            _worldMatrix = CC3Matrix.CreateWorldMatrix(this.WorldPosition + this.WorldTranslationChangeNeededToUpdate,
                                                       _worldScale,
                                                       this.LocalRotation,
                                                       _rotationChangeRelativeToAnchorNeededToUpdate,
                                                       _rotationAnchorPointRelativeToPositionUsedForUpdate);

            this.FinishedUpdatingWorldMatrix();
        }

        protected override void FinishedUpdatingWorldMatrix()
        {
            _rotationChangeRelativeToAnchorNeededToUpdate = CC3Quaternion.CC3QuaternionIdentity;
            _rotationAnchorPointRelativeToPositionUsedForUpdate = CC3Vector.CC3VectorZero;

            base.FinishedUpdatingWorldMatrix();
        }

        #endregion Updating world matrix methods


        public virtual void Draw()
        {

        }

    }
}

