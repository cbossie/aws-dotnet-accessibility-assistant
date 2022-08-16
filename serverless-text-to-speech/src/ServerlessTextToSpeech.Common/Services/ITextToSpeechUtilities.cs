using Amazon.Textract.Model;
using ServerlessTextToSpeech.Common.Model;

namespace ServerlessTextToSpeech.Common.Services;

public interface ITextToSpeechUtilities
{
    TextDocument GetTextDocument(List<Block> blocks);

}
