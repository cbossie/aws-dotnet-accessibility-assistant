﻿using Amazon.Textract.Model;
using ServerlessTextToSpeech.Common.Model;

namespace ServerlessTextToSpeech.Common.Services;

public class TextToSpeechUtilities : ITextToSpeechUtilities
{
    public TextDocument GetTextDocument(List<Block> blocks)
    {
        var doc = new TextDocument();


        foreach (var block in blocks)
        {
            if (block.BlockType == Amazon.Textract.BlockType.LINE)
            {
                doc.AddText(block.Text);
            }
        }
        return doc;
    }
}
