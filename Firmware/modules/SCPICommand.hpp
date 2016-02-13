/**
 * @file SCPICommand.hpp
 *
 * @date 11.12.2015
 * @author Andre
 * @description
 */
#ifndef SCPI_COMMAND_HPP_
#define SCPI_COMMAND_HPP_

#include <stdint.h>
#include <stdbool.h>

#define SCPI_COMMAND_MAX_TOKENS   10

class SCPICommand
{
public:
  SCPICommand();
  SCPICommand(char* scpiCommand, int scpiCommandLength);
  virtual ~SCPICommand() { }

  void parseSCPICommand(char* scpiCommand, int scpiCommandLength);

  const char* getSCPIParam();
  static int getSCPICommand(const char *scpiToken, char *scpiCommand, int scpiCommandLength, bool& query);

  bool isQuery();

  /** Gets all parsed SCPI tokens.
   *
   * @return pointer to a string (char*) array with tokens
   */
  char** getSCPITokens()
  {
    return _tokens;
  }

  /** Gets the number of found tokens.
   *
   * @return number of found tokens
   */
  int getSCPITokenCount()
  {
    return _tokensFound;
  }

private:
  int tokenize(char *src, int length, char delimiter, char *tokens[], int maxTokens);

  char* _tokens[SCPI_COMMAND_MAX_TOKENS];
  int _tokensFound;
};


#endif /* SCPI_COMMAND_HPP_ */
