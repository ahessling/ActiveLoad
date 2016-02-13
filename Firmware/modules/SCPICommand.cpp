/**
 * @file SCPICommand.cpp
 *
 * @date 11.12.2015
 * @author Andre
 * @description
 */
#include "SCPICommand.hpp"
#include <string.h>

SCPICommand::SCPICommand()
{
  _tokensFound = 0;
}

SCPICommand::SCPICommand(char* scpiCommand, int scpiCommandLength)
{
  parseSCPICommand(scpiCommand, scpiCommandLength);
}

bool SCPICommand::isQuery()
{
  bool query;

  // determine length of last token
  const char *lastToken = _tokens[_tokensFound - 1];
  int scpiTokenLength = strlen(lastToken);

  // determine if the last scpi command is a query (? at the end of the string)
  if (scpiTokenLength > 1)
  {
    query = (lastToken[scpiTokenLength - 1] == '?') ? true : false;
  }
  else
  {
    query = false;
  }

  return query;
}

void SCPICommand::parseSCPICommand(char* scpiCommand, int scpiCommandLength)
{
  // Splits SCPI command lines like :Layer1:Layer2:Command into tokens
    _tokensFound = tokenize(scpiCommand, scpiCommandLength, ':', _tokens, SCPI_COMMAND_MAX_TOKENS);
}

/** Splits a string into tokens using a delimiter.
 *
 * @note The source string will be modified!
 *
 * @param src source buffer string
 * @param length length of source buffer
 * @param delimiter delimiter character
 * @param tokens pointer to a char* array which should hold the string tokens
 * @param maxTokens maximum number of tokens in array
 * @return number of found tokens
 */
int SCPICommand::tokenize(char *src, int length, char delimiter, char *tokens[], int maxTokens)
{
  int tokensFound = 0;
  char *lastTokenPos = src;

  while ((length-- > 0) && (tokensFound < maxTokens))
  {
    if (*src == delimiter)
    {
      *src = '\0';
      tokens[tokensFound++] = lastTokenPos;
      lastTokenPos = src;
      lastTokenPos++;
    }

    src++;
  }

  // save last token which has no delimiter due to end of string
  if (tokensFound < maxTokens)
  {
    tokens[tokensFound++] = lastTokenPos;
  }

  return tokensFound;
}

/** Gets the short form (uppercase characters) of an SCPI command.
 *
 * This function copies the uppercase characters of an SCPI command
 * and ignores all other characters except for '*'.
 *
 * @param scpiToken source buffer string
 * @param scpiCommand destination buffer string with uppercase command
 * @param scpiCommandLength maximum number of bytes in scpi_command
 * @param query pointer to a bool which indicates if command is a query
 * @return length of found command string
*/
int SCPICommand::getSCPICommand(const char *scpiToken, char *scpiCommand, int scpiCommandLength, bool& query)
{
  int count = 0;
  int scpiTokenLength = strlen(scpiToken);

  // determine if the scpi command is a query (? at the end of the string)
  if (scpiTokenLength > 1)
  {
    query = (scpiToken[scpiTokenLength - 2] == '?') ? true : false;
  }
  else
  {
    query = false;
  }

  // copy all uppercase characters and stop at everything else
  while (*scpiToken && scpiCommandLength-- > 0)
  {
    if ((*scpiToken >= 'A' && *scpiToken <= 'Z') || *scpiToken == '*')
    {
      *scpiCommand++ = *scpiToken;
    }
    else
    {
      break;
    }

    scpiToken++;
    count++;
  }

  // null termination if possible
  if (scpiCommandLength > 0)
  {
    *scpiCommand = '\0';
  }

  return count;
}

/** Gets the (optional) parameter of an SCPI command.
  *
  * @return pointer to the beginning of the parameter in scpi_line; NULL if no parameter is found.
*/
const char* SCPICommand::getSCPIParam()
{
  const char *paramStart = 0;

  if (_tokensFound > 0)
  {
    // point to last token which holds the (optional) parameter
    const char *scpiCommand = _tokens[_tokensFound - 1];

    while (*scpiCommand)
    {
      if (*scpiCommand == ' ')
      {
        paramStart = scpiCommand;
        paramStart++;
        break;
      }

      scpiCommand++;
    }
  }

  return paramStart;
}
