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
using System.Diagnostics;

namespace Cocos3D
{
    public class LCC3Identifiable
    {
        // Static fields

        private static int _instanceCount = 0;
        private static int _lastAssignedTag = 0;

        // Instance fields

        private int _tag;
        private string _name;
        private object _userData;

        #region Properties

        public static int InstanceCount 
        { 
            get { return _instanceCount; }
        }

        public int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public object UserData
        {
            get { return _userData; }
            set { _userData = value; }
        }

        public virtual string NameSuffix
        {
            get 
            { 
                Debug.Assert(true, String.Format("{0} must override the NameSuffix property.", this.GetType().ToString()));
                return null; 
            }
        }

        #endregion Properties


        #region Constructors

        public LCC3Identifiable(int tag, string name)
        {
            LCC3Identifiable._instanceCount ++;

            _tag = tag;
            _name = name;

            this.InitUserData();
        }

        public LCC3Identifiable(int tag) : this(tag, null)
        {

        }

        public LCC3Identifiable(string name) : this(0, name)
        {
            _tag = this.NextTag();
        }

        public LCC3Identifiable() : this(null)
        {

        }

        #endregion Constructors


        #region Setting up tags and names

        protected static void ResetTagAllocation() 
        { 
            LCC3Identifiable._lastAssignedTag = 0; 
        }

        protected virtual int NextTag()
        {
            return ++LCC3Identifiable._lastAssignedTag;
        }

        #endregion Setting up tags and names


        #region Setting up user data

        protected virtual void InitUserData()
        {
            // Overriden by subclasses
        }

        protected virtual void ReleaseUserData()
        {
            // Overriden by subclasses
        }

        #endregion Setting up user data


        #region Populating and copying

        public virtual bool ShouldIncludeInDeepCopy()
        {
            return true;
        }

        public virtual void PopulateFrom(LCC3Identifiable another)
        {
            this.CopyUserDataFrom(another);
        }

        public virtual void CopyUserDataFrom(LCC3Identifiable another)
        {
            // Overriden by subclasses
        }

        public LCC3Identifiable CopyWithName<T>(string name) where T: LCC3Identifiable
        {
            LCC3Identifiable copy = (LCC3Identifiable)Activator.CreateInstance(typeof(T), new string[] { name });
            copy.PopulateFrom(this);

            return copy;
        }

        public LCC3Identifiable Copy<T>() where T: LCC3Identifiable
        {
            return this.CopyWithName<T>(this.Name);
        }

        #endregion Populating and copying


        #region Description

        public override string ToString()
        {
            string name = (_name != null) ? _name : "Unnamed";
            return String.Format("{0} {1}:{2}", this.GetType().ToString(), name, _tag);
        }

        public virtual string FullDescription()
        { 
            return this.ToString();
        }

        #endregion Description
    }
}

