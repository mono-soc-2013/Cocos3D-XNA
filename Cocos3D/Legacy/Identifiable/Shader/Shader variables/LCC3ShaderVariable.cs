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

namespace Cocos3D
{
    public class LCC3ShaderVariable
    {
        // ivars

        protected LCC3ShaderProgram _program;
        protected LCC3ElementType _type;
        protected string _name;
        protected int _index;
        protected LCC3Semantic _semantic;
        protected uint _semanticIndex;
        protected uint _size;
        LCC3VertexAttrIndex _location;
        LCC3ShaderVariableScope _scope;

        #region Properties

        public LCC3ShaderProgram Program
        {
            get { return _program; }
        }

        public LCC3ElementType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Name
        {
            get { return _name; }
        }

        public LCC3VertexAttrIndex Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public LCC3Semantic Semantic
        {
            get { return _semantic; }
            set { _semantic = value; }
        }

        public uint SemanticIndex
        {
            get { return _semanticIndex; }
            set { _semanticIndex = value; }
        }

        public LCC3ShaderVariableScope Scope
        {
            get { return _scope; }
            set { _scope = value; }
        }

        public virtual uint Size
        {
            get { return _size; }
            set { _size = value; }
        }

        #endregion Properties


        #region Allocation and initialization

        public LCC3ShaderVariable()
        {

        }

        public LCC3ShaderVariable(LCC3ShaderProgram program, int index) : this()
        {
            this.LoadInProgramAtIndex(program, index);
        }

        internal void LoadInProgramAtIndex(LCC3ShaderProgram program, int index)
        {
            _index = index;
            _semantic = LCC3Semantic.SemanticNone;
            _semanticIndex = 0;
            _scope = LCC3ShaderVariableScope.ScopeUnknown;
            _program = program;
            this.PopulateFromProgram();
        }

        public T CloneVariable<T>() where T : LCC3ShaderVariable, new()
        {
            T clonedVariable = new T();
            clonedVariable.LoadInProgramAtIndex(_program, _index);
            clonedVariable.PopulateFrom(this);
            return clonedVariable;
        }

        public virtual void PopulateFromProgram()
        {
            // Overriden by subclasses
        }

        public virtual void PopulateFrom(LCC3ShaderVariable variable)
        {
            _name = variable.Name;
            _location = variable.Location;
            _size = variable.Size;
            _semantic = variable.Semantic;
            _semanticIndex = variable._semanticIndex;
            _scope = variable.Scope;
        }

        #endregion Allocation and initialization

    }
}

