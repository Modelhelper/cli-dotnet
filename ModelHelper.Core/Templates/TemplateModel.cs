using System;
using System.Collections.Generic;
using ModelHelper.Core.Database;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Project;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Templates
{
    public class TemplateModel : ITemplateModel
    {
        public TemplateModel()
        {
            Options = new Dictionary<string, object>();
        }
        public IEntity Table { get; set; }
        public IProject Project { get; set; }
        public Dictionary<string, object> Options { get; set; }
        public List<ISqlGenerator> SqlScriptGenerators { get; set; }
        public List<IDatatypeConverter> DatatypeConverters { get; set; }

        public bool IncludeChildren { get; set; }
        public bool IncludeParents { get; set; }
        public bool IncludeRelations { get; set; }
    }

    public class GeneratedTemplate
    {
        public GeneratedTemplate()
        {
            Identifier = Guid.NewGuid();
            State = TemplateGeneratedState.New;
            SourceFile = new SourceFile();
            DestinationFile = new DestinationFile();
        }
        public Guid Identifier { get; set; }
        public TemplateFile TemplateUsed { get; set; }
        public IEntity TableUsed { get; set; }
        
        public CreateCodeContentMode CodeContentMode { get; set; }

        public SourceFile SourceFile { get; set; }
        public DestinationFile DestinationFile { get; set; }

        public TemplateGeneratedState State { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime CompletedOn { get; set; }

        public int Index { get; set; }
        // new, existing
        // overwrite, do nothing, ask

        // snippet exists
        // if exists and has diff - extract differences and insert code?
    }

    public class SourceFile
    {
        public string Content { get; set; }
        public string Filename { get; set; }

        public bool CanExport { get; set; }

    }

    public class DestinationFile
    {
        public string Content { get; set; }
        public string Path { get; set; }

        public bool Exists { get; set; }

        public bool IsDiff { get; set; }
    }

    public enum CreateCodeContentMode
    {
        CreateFile,
        InsertToFile
        
    }

    public enum TemplateGeneratedState
    {
        New,
        InProgress,
        CodeGeneratedSuccess,
        CodeGeneratedFail,
        WrittenToFileSuccess,
        WrittenToFileFail,
        InsertedToCodeSuccess,
        InsertedToCodeFail,
        Completed
    }
}