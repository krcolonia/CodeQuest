using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

public class PrintMethod : Node
{
  // ! WARNING: Deprecated, tinabi ko na lang bilang basis sa pag-ayos ko pa ng modules ng CQScript.

  private List<string> output = new List<string>();
  private List<string> syntaxList = new List<string>();

  private bool error = false;
  private string syntaxFormat;

  private string ConsolePrint = "";

  public string Print(string expression)
  {
    ClearVars();

    List<string> splitted = SplitString(expression);

    bool lexValid = true;

    lexValid = Lexical(splitted);

    if (lexValid)
    {
      Semantic(syntaxList);
    }

    return ConsolePrint;
  }

  private void ClearVars()
  {
    output.Clear();
    syntaxList.Clear();
    ConsolePrint = "";
  }

  private bool Semantic(List<string> syntaxList)
  {
    if ((syntaxList[0] == "print" || syntaxList[0] == "println") && int.TryParse(syntaxList[2], out int intValue))
    {
      ConsolePrint += syntaxList[2];
      GD.Print(syntaxList[2]);
      return true;
    }
    else if ((syntaxList[0] == "print" || syntaxList[0] == "println") && syntaxList[2].StartsWith("\"") && syntaxList[2].EndsWith("\""))
    {
      ConsolePrint += syntaxList[2].Substring(1, syntaxList[2].Length - 2);
      GD.Print(syntaxList[2].Substring(1, syntaxList[2].Length - 2) );
      return true;
    }
    else if ((syntaxList[0] == "print" || syntaxList[0] == "println") && syntaxList[2].Length == 3 && syntaxList[2][0] == '\'' && syntaxList[2][2] == '\'')
    {
      ConsolePrint += syntaxList[2].Substring(1, syntaxList[2].Length - 2);
      GD.Print(syntaxList[2].Substring(1, syntaxList[2].Length - 2));
      return true;
    }
    else if ((syntaxList[0] == "print" || syntaxList[0] == "println") && double.TryParse(syntaxList[2], out double doubleValue))
    {
      ConsolePrint += syntaxList[2];
      GD.Print(syntaxList[2]);
      return true;
    }
    else
    {
      GD.Print("Semantically Incorrect");
      ConsolePrint += "ERROR: Syntax Error. Please check the value to be printed out.";
    }
    return false;
  }
  private bool Lexical(List<string> splited)
  {
    foreach (string item in splited)
    {
      if (item.Equals(";"))
      {
        Tokenizer(item);
      }
      else if (item.EndsWith(";"))
      {
        string noSemiColon = item.Substring(0, item.Length - 1);
        Tokenizer(noSemiColon);

        string semiColon = ";";
        Tokenizer(semiColon);
      }
      else
      {
        Tokenizer(item);
      }
    }

    if (error)
    {
      GD.Print("Lexically Incorrect");
      ConsolePrint += "ERROR: Lexical Error. Please check if you've spelled the \'print\' command properly.";
      return false;
    }
    else
    {
      // GD.Print("Lexically Correct");
      return true;
    }
  }

  private void Tokenizer(string item)
  {
    switch(item)
    {
      case "print":
      case "println":
        syntaxList.Add(item);
        output.Add("<method>");
        break;
      case "(":
        syntaxList.Add(item);
        output.Add("<oparen>");
        break;
      case ")":
        syntaxList.Add(item);
        output.Add("<cparen>");
        break;
      // case "int":
      // case "double":
      // case "char":
      // case "string":
      //   syntaxList.Add(item);
      //   output.Add("<data_type>");
      //   break;
      case "=":
        syntaxList.Add(item);
        output.Add("<assignment_operator>");
        break;
      case ";":
        syntaxList.Add(item);
        output.Add("<delimiter>");
        break;
      case string IntVal when Regex.IsMatch(IntVal, "-?\\d+"):
      case string StrVal when Regex.IsMatch(StrVal, "\"[^\"]*\""):
      case string ChrVal when Regex.IsMatch(ChrVal, "'.'"):
      case string DblVal when Regex.IsMatch(DblVal, "-?\\d+(\\.\\d+)?"):
        syntaxList.Add(item);
        output.Add("<value>");
        break;
      case string ident when !ident.Contains("\""):
        break;
      default:
        error = true;
        break;
    }
  }

  // private void Tokenizer(string item)
  // {
  //   if (item == "print" || item == "println")
  //   {
  //     syntaxList.Add(item);
  //     output.Add("<method>");
  //   }
  //   else if (item == "(")
  //   {
  //     syntaxList.Add(item);
  //     output.Add("<oparen>");
  //   }
  //   else if (Regex.IsMatch(item, "-?\\d+") || Regex.IsMatch(item, "\"[^\"]*\"") || Regex.IsMatch(item, "'.'") || Regex.IsMatch(item, "-?\\d+(\\.\\d+)?"))
  //   {
  //     syntaxList.Add(item);
  //     output.Add("<value>");
  //   }
  //   else if (item == ")")
  //   {
  //     syntaxList.Add(item);
  //     output.Add("<cparen>");
  //   }
  //   else if (item == ";")
  //   {
  //     syntaxList.Add(item);
  //     output.Add("<delimiter>");
  //   }
  //   else if (!output.Contains("\""))
  //   {
  //     syntaxList.Add(item);
  //     output.Add("<identifier>");
  //   }
  //   else
  //   {
  //     error = true;
  //   }
  // }

  private List<string> SplitString(string input)
  {
    List<string> split = new List<string>();
    string parenSplit = @"(\()|(\))";

    split = new List<string>(Regex.Split(input, parenSplit));

    split.RemoveAll(string.IsNullOrEmpty);

    split.ConvertAll(part => part.Trim());

    foreach (string item in split)
    {
      GD.Print("\'" + item + "\'");
    }

    return split;
  }
}
