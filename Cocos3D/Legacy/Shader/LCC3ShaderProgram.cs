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
    public class LCC3ShaderProgram
    {
        // Static fields

        private static uint _lastAssignedProgramTag = 0;

        // Instance fields

        #region Allocation and initialization

        public LCC3ShaderProgram(uint tag, string name)
        {
        }

        public LCC3ShaderProgram(string name, ILCC3ShaderSemanticDelegate semanticDelegate, string shaderFilename)
        {
        }

        #endregion Allocation and initialization


        #region Tag allocation

        public static void ResetTagAllocation()
        {
            _lastAssignedProgramTag = 0;
        }

        public uint NextTag()
        {
            return ++_lastAssignedProgramTag;
        }

        #endregion Tag allocation


        #region Variables

        public LCC3ShaderUniform UniformNamed(string varName)
        {
            return null;
        }

        public LCC3ShaderUniform UniformAtLocation(int uniformLocation)
        {
            return null;
        }

        public LCC3ShaderUniform UniformForSemantic(LCC3SemanticVertex semantic)
        {
            return null;
        }

        public LCC3ShaderAttribute AttributeNamed(string varName)
        {
            return null;
        }

        public LCC3ShaderAttribute AttributeAtLocation(int attributeLocation)
        {
            return null;
        }

        public LCC3ShaderAttribute AttributeForSemantic(LCC3SemanticVertex semantic)
        {
            return null;
        }

        public LCC3ShaderAttribute AttributeForSemantic(LCC3SemanticVertex semantic, uint semanticIndex)
        {
            return null;
        }

        public void MarkSceneScopeDirty()
        {

        }

        public void WillBeginDrawingScene()
        {

        }

        #endregion Variables


        #region Compiling and linking

        public void CompileAndLinkShaderFile(string shaderFilename)
        {

        }

        public string PlatformPreamble()
        {
            return null;
        }

        public void ConfigureUniforms()
        {

        }

        public void AddUniform(LCC3ShaderUniform uniform)
        {

        }

        public void ConfigureAttributes()
        {

        }

        #endregion Compiling and linking


        #region Binding

        public void BindWithVisitor(LCC3NodeDrawingVisitor visitor)
        {

        }

        public void PopulateVertexAttributesWithVisitor(LCC3NodeDrawingVisitor visitor)
        {

        }

        public void PopulateSceneScopeUniformsWithVisitor(LCC3NodeDrawingVisitor visitor)
        {

        }

        public void PopulateNodeScopeUniformsWithVisitor(LCC3NodeDrawingVisitor visitor)
        {

        }

        public void PopulateDrawScopeUniformsWithVisitor(LCC3NodeDrawingVisitor visitor)
        {

        }

        public void PopulateUniformsWithVisitor(LCC3ShaderUniform[] uniforms, LCC3NodeDrawingVisitor visitor)
        {

        }

        #endregion Binding


        #region Program cache

        public static void AddProgram(LCC3ShaderProgram program)
        {

        }

        public static LCC3ShaderProgram GetProgramNamed(string name)
        {
            return null;
        }

        public static void RemoveProgram(LCC3ShaderProgram program)
        {

        }

        public static void RemoveProgramNamed(string name)
        {

        }


        #endregion Program cache
    }
}

