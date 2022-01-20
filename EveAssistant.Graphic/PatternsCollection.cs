using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EveAssistant.Common;

namespace EveAssistant.Graphic
{
    public class PatternsCollection : IPatternsCollection
    {
        public List<IPattern> PatternEntity { get; set; } = new List<IPattern>();

        private readonly ImageFactory _imageFactory = new ImageFactory();

        public PatternsCollection()
        {
           
        }

        public PatternsCollection(DirectoryInfo currentDirectory) : this()
        {
            foreach (var fileInfo in currentDirectory.GetFiles().Where(f => !(f.FullName.EndsWith(".jpg") || f.Name.EndsWith(".txt"))))
            {
                var pattern = new Pattern
                {
                    Image = _imageFactory.Load(fileInfo.FullName),
                    Name = fileInfo.Name.Replace(fileInfo.Extension, "")
                };

                PatternEntity.Add(pattern);
            }
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}