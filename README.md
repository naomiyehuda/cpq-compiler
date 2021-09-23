# CPQ Compiler
This project compiles CPL, which is a very small subset of C, to an assembly language called Quad.  
The compiler was developed using [ANTLR](https://www.antlr.org/) tool and C# programming language.

## Usage

To compile a CPL file via command prompt, simply drag CPQ.exe from bin folder (after build succeed) as following:

    cpq.exe fileToBeCompiled.ou

The program will create a `.qud` output file in the same directory.

## Building

### Requirements

The only requirement for building the project is to have [Visual Srudio 2019](https://visualstudio.microsoft.com/vs/community/). (for avoidance of full installation, check C# only)

## Folders in this directory
<pre>
doc:    an explanation about the compiler and description of the project (starting at page 35).  
src:    the source files to this compiler  
tests:	input and output examples of the compiler
</pre>
