﻿Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.Diagnostics
Imports Microsoft.CodeAnalysis.VisualBasic
Imports System.Collections.Immutable

Namespace Design
    <DiagnosticAnalyzer(LanguageNames.VisualBasic)>
    Public Class CatchEmptyAnalyzer
        Inherits DiagnosticAnalyzer

        Public Const DiagnosticId As String = "CC0003"
        Public Const Title As String = "Your catch may includes some Exception"
        Public Const MessageFormat As String = "{0}"
        Public Const Category As String = SupportedCategories.Design
        Protected Shared Rule As DiagnosticDescriptor = New DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault:=True,
            helpLink:=HelpLink.ForDiagnostic(DiagnosticId))

        Public Overrides ReadOnly Property SupportedDiagnostics() As ImmutableArray(Of DiagnosticDescriptor) = ImmutableArray.Create(Rule)

        Public Overrides Sub Initialize(context As AnalysisContext)
            context.RegisterSyntaxNodeAction(AddressOf Analyzer, SyntaxKind.CatchStatement)
        End Sub

        Private Sub Analyzer(context As SyntaxNodeAnalysisContext)
            Dim catchStatement = DirectCast(context.Node, Syntax.CatchStatementSyntax)
            If catchStatement Is Nothing Then Exit Sub

            If catchStatement.IdentifierName Is Nothing Then
                Dim diag = Diagnostic.Create(Rule, catchStatement.GetLocation(), "Consider including an Exception Class in catch.")
                context.ReportDiagnostic(diag)
            End If
        End Sub

    End Class
End Namespace