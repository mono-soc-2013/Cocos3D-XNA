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
        private CC3Vector _worldTranslationChangeNeededToUpdate;

        protected CC3Node _nodeBeingObservedForTransformChanges;
        protected List<ICC3NodeTransformObserver> _listOfNodeTransformObservers;


        #region Properties

        // Instance properties

        public CC3Vector WorldPosition
        {
            get { return _worldMatrix.TranslationOfTransformMatrix(); }
            set
            {
                CC3Vector positionChange = value - this.WorldPosition;
                this.IncrementallyUpdateWorldTranslation(positionChange);                              
            }
        }
       
        protected CC3Vector WorldTranslationChangeNeededToUpdate
        {
            get { return _worldTranslationChangeNeededToUpdate; }
            set { _worldTranslationChangeNeededToUpdate = value; }
        }

        #endregion Properties


        #region Constructors

        public CC3Node()
        {
            _worldTranslationChangeNeededToUpdate = CC3Vector.CC3VectorZero;
            _worldMatrix = CC3Matrix.CC3MatrixIdentity;
            _listOfNodeTransformObservers = new List<ICC3NodeTransformObserver>();
        }

        public CC3Node(CC3Vector worldPosition) : this()
        {
            _worldMatrix = CC3Matrix.CreateTranslationMatrix(worldPosition);
        }

        #endregion Constructors


        #region Updating world matrix methods

        internal void IncrementallyUpdateWorldTranslation(CC3Vector translationChange)
        {
            _worldTranslationChangeNeededToUpdate = translationChange;

            this.ShouldUpdateWorldMatrix();

            foreach (ICC3NodeTransformObserver transformObserver in _listOfNodeTransformObservers)
            {
                transformObserver.ObservedNodeWorldTranslationDidChange(this, translationChange);
            }
        }

        // Give subclasses an opportunity to do something else. e.g. see CC3Camera
        protected virtual void ShouldUpdateWorldMatrix()
        {
            this.UpdateWorldMatrix();
        }

        protected virtual void UpdateWorldMatrix()
        {
            _worldMatrix = CC3Matrix.CreateTranslationMatrix(this.WorldPosition + this.WorldTranslationChangeNeededToUpdate);
        }

        protected virtual void FinishedUpdatingWorldMatrix()
        {
            _worldTranslationChangeNeededToUpdate = CC3Vector.CC3VectorZero;
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

        public void ObservedNodeWorldTranslationDidChange(CC3Node node, 
                                                          CC3Vector translationChange)
        {
            if (node == _nodeBeingObservedForTransformChanges)
            {
                this.IncrementallyUpdateWorldTranslation(translationChange);
            }
        }

        public void ObservedNodeWorldTransformDidChange(CC3Node node, 
                                                        CC3Vector translationChange,
                                                        CC3Vector scaleChange,
                                                        CC3Quaternion rotationChangeRelativeToPosition,
                                                        CC3Vector rotationAnchorPointRelativeToPosition)
        {
            this.ObservedNodeWorldTranslationDidChange(node, translationChange);
        }

        #endregion Node transform listener interface methods
    }
}
