using Amazon.Textract.Model;
using ServerlessTextToSpeech.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessTextToSpeech.Common.Services;

public class TextToSpeechUtilities : ITextToSpeechUtilities
{
    public TextDocument GetTextDocument(List<Block> blocks)
    {
        var doc = new TextDocument();
        doc.AddText("Testing");
        return doc;
    }
}
