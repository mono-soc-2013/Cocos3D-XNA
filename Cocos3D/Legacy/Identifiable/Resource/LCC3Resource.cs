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
    public abstract class LCC3Resource : LCC3Identifiable
    {
        // Static vars
        static int _lastAssignedResourceTag = 0;
        static Dictionary<string, LCC3Resource> _resourcesByName = new Dictionary<string, LCC3Resource>();

        // ivars

        bool _wasLoaded;

        #region Allocation and initialization

        public LCC3Resource(int tag, string name) : base(tag, name)
        {

        }

        public LCC3Resource(string filePath) : this(0, filePath)
        {
            _wasLoaded = this.ProcessFile(filePath);
        }

        #endregion Allocation and initialization

        #region Tag allocation

        public new static void ResetTagAllocation()
        {
            _lastAssignedResourceTag = 0;
        }

        protected override int NextTag()
        {
            return ++_lastAssignedResourceTag;
        }

        #endregion Tag allocation


        #region Processing file

        public abstract bool ProcessFile(string absFilePath);

        public abstract bool SaveToFile(string filePath);

        #endregion Processing file


        #region Resource cache

        public LCC3Resource GetResourceNamed(string resName)
        {
            return _resourcesByName[resName];
        }

        public void AddResource(LCC3Resource resource)
        {
            if (resource != null)
            {
                _resourcesByName[resource.Name] = resource;
            }
        }

        public void RemoveResource(LCC3Resource resource)
        {
            if (resource != null)
            {
                _resourcesByName.Remove(resource.Name);
            }
        }

        public void RemoveAllResources()
        {
            _resourcesByName.Clear();
        }

        #endregion Resource cache
    }
}

