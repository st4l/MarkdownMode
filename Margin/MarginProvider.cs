﻿using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace MarkdownMode
{
    [Export(typeof(IWpfTextViewMarginProvider))]
    [Name(Margin.Name)]
    [MarginContainer(PredefinedMarginNames.Top)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [ContentType(ContentType.Name)]
    sealed class MarginProvider : IWpfTextViewMarginProvider
    {
        [Import]
        System.IServiceProvider GlobalServiceProvider = null;

        public IWpfTextViewMargin CreateMargin(IWpfTextViewHost wpfTextViewHost, IWpfTextViewMargin marginContainer)
        {
            ITextDocument document;
            if (!wpfTextViewHost.TextView.TextDataModel.DocumentBuffer.Properties.TryGetProperty(typeof(ITextDocument), out document))
            {
                document = null;
            }

            MarkdownPackage package = null;

            // If there is a shell service (which there should be, in VS), force the markdown package to load
            IVsShell shell = GlobalServiceProvider.GetService(typeof(SVsShell)) as IVsShell;
            if (shell != null)
                package = MarkdownPackage.ForceLoadPackage(shell);

            return new Margin(wpfTextViewHost.TextView, package, document);
        }
    }
}
