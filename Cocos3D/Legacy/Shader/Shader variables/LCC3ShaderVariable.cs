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
        // Instance fields

        protected LCC3ShaderProgram _program;
        protected string _name;
        private int _location;
        protected int _index;
        protected LCC3SemanticVertex _semanticVertex;
        protected uint _semanticVertexIndex;
        private LCC3ShaderVariableScope _scope;
        private uint _size;

        #region Properties

        public LCC3ShaderProgram Program
        {
            get { return _program; }
        }

        public string Name
        {
            get { return _name; }
        }

        public int Location
        {
            get { return _location; }
        }

        public LCC3SemanticVertex SemanticVertex
        {
            get { return _semanticVertex; }
        }

        public uint SemanticVertexIndex
        {
            get { return _semanticVertexIndex; }
        }

        public LCC3ShaderVariableScope Scope
        {
            get { return _scope; }
        }

        public uint Size
        {
            get { return _size; }
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
            _semanticVertex = LCC3SemanticVertex.SemanticNone;
            _semanticVertexIndex = 0;
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
            _semanticVertex = variable.SemanticVertex;
            _semanticVertexIndex = variable._semanticVertexIndex;
            _scope = variable.Scope;
        }

        #endregion Allocation and initialization

    }
}

