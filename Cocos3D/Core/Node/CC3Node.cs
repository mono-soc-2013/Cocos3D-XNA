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
using System.Diagnostics;

namespace Cocos3D
{
    public abstract class CC3Node : ICC3NodeTransformObserver
    {
        // Instance fields

        protected CC3Matrix _worldMatrix;
        protected CC3Vector _worldScale;

        private CC3Vector _worldTranslationChangeNeededToUpdate;
        private CC3Quaternion _rotationChangeRelativeToAnchorNeededToUpdate;
        private CC3Vector _rotationAnchorPointRelativeToPositionUsedForUpdate;

        private CC3Node _nodeBeingObservedForTransformChanges;
        private List<ICC3NodeTransformObserver> _listOfNodeTransformObservers;

        #region Properties

        // Instance properties

        public CC3Vector WorldPosition
        {
            get { return _worldMatrix.TranslationOfTransformMatrix(); }
            set
            {
                CC3Vector positionChange = value - this.WorldPosition;
                this.IncrementallyUpdateWorldTransform(positionChange, 
                                                       CC3Vector.CC3VectorZero,
                                                       CC3Quaternion.CC3QuaternionZero,
                                                       CC3Vector.CC3VectorZero);
            }
        }

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

        protected CC3Vector WorldTranslation
        {
            get { return this.WorldPosition + _worldTranslationChangeNeededToUpdate; }
        }
       
        #endregion Properties


        #region Constructors

        public CC3Node()
        {
            _worldTranslationChangeNeededToUpdate = CC3Vector.CC3VectorZero;
            _rotationChangeRelativeToAnchorNeededToUpdate = CC3Quaternion.CC3QuaternionIdentity;
            _rotationAnchorPointRelativeToPositionUsedForUpdate = CC3Vector.CC3VectorZero;

            _worldScale = CC3Vector.CC3VectorUnitCube;
            _worldMatrix = CC3Matrix.CC3MatrixIdentity;
            _listOfNodeTransformObservers = new List<ICC3NodeTransformObserver>();
        }

        public CC3Node(CC3Vector worldPosition) : this()
        {
            _worldMatrix = CC3Matrix.CreateWorldMatrix(worldPosition,
                                                       _worldScale,
                                                       CC3Quaternion.CC3QuaternionIdentity,
                                                       CC3Quaternion.CC3QuaternionIdentity,
                                                       CC3Vector.CC3VectorZero);
        }

        #endregion Constructors


        #region Updating world matrix methods

        internal void IncrementallyUpdateWorldTransform(CC3Vector translationChange, 
                                                        CC3Vector scaleChange, 
                                                        CC3Quaternion rotationChangeRelativeToAnchor,
                                                        CC3Vector rotationAnchorPointRelativeToPosition)
        {
            _worldTranslationChangeNeededToUpdate = translationChange;
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

        // Give subclasses an opportunity to do something else. e.g. see CC3Camera
        protected virtual void ShouldUpdateWorldMatrix()
        {
            this.UpdateWorldMatrix();
        }

        protected void UpdateWorldMatrix()
        {
            _worldMatrix = CC3Matrix.CreateWorldMatrix(this.WorldTranslation,
                                                       _worldScale,
                                                       this.LocalRotation,
                                                       _rotationChangeRelativeToAnchorNeededToUpdate,
                                                       _rotationAnchorPointRelativeToPositionUsedForUpdate);

            this.FinishedUpdatingWorldMatrix();
        }

        private void FinishedUpdatingWorldMatrix()
        {
            _worldTranslationChangeNeededToUpdate = CC3Vector.CC3VectorZero;
            _rotationChangeRelativeToAnchorNeededToUpdate = CC3Quaternion.CC3QuaternionIdentity;
            _rotationAnchorPointRelativeToPositionUsedForUpdate = CC3Vector.CC3VectorZero;
        }

        #endregion Updating world matrix methods


        #region Transform observer management methods

        public void RegisterToObserveTransformChangesOfNode(CC3Node nodeToObserveTransformChanges)
        {
            Debug.Assert(_listOfNodeTransformObservers.Contains(nodeToObserveTransformChanges) == true, 
                         @"Attempting to register to observe a node that is itself observing this object, 
                            which would create a circular dependency.");

            if (_nodeBeingObservedForTransformChanges != null)
            {
                _nodeBeingObservedForTransformChanges.RemoveTransformObserver(this);
            }

            _nodeBeingObservedForTransformChanges = nodeToObserveTransformChanges;

            if(_nodeBeingObservedForTransformChanges != null)
            {
                _nodeBeingObservedForTransformChanges.AddTransformObserver(this);
            }
        }

        internal void AddTransformObserver(ICC3NodeTransformObserver transformObserver)
        {
            if (_listOfNodeTransformObservers.Contains(transformObserver) == false)
            {
                _listOfNodeTransformObservers.Add(transformObserver);
            }
        }

        internal void RemoveTransformObserver(ICC3NodeTransformObserver transformObserver)
        {
            _listOfNodeTransformObservers.Remove(transformObserver);
        }

        #endregion Transform listening management methods


        #region Node transform listener interface methods

        public void ObservedNodeWorldTransformDidChange(CC3Node node, 
                                                        CC3Vector translationChange,
                                                        CC3Vector scaleChange,
                                                        CC3Quaternion rotationChangeRelativeToPosition,
                                                        CC3Vector rotationAnchorPointRelativeToPosition)
        {
            if (node == _nodeBeingObservedForTransformChanges)
            {
                this.IncrementallyUpdateWorldTransform(translationChange, 
                                                       scaleChange, 
                                                       rotationChangeRelativeToPosition,
                                                       rotationAnchorPointRelativeToPosition);
            }
        }

        #endregion Node transform listener interface methods
    }
}

