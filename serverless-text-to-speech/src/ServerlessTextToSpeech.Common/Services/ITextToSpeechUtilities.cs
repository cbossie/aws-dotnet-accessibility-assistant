using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Textract.Model;
using ServerlessTextToSpeech.Common.Model;

namespace ServerlessTextToSpeech.Common.Services;

public interface ITextToSpeechUtilities
{
    TextDocument GetTextDocument(List<Block> blocks);


}
