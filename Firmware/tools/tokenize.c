/**
 * @file tokenize.c
 *
 * @date 10.12.2015
 * @author Andre
 * @description
 */
#include <stdio.h>
#include <string.h>
#include "tokenize.h"

/** Splits a string into tokens using a delimiter.
 *
 * @note The source string will be modified!
 * 
 * @param src source buffer string
 * @param length length of source buffer
 * @param delimiter delimiter character
 * @param tokens pointer to a char* array which should hold the string tokens
 * @param max_tokens maximum number of tokens in array
 * @return number of found tokens
 */
int tokenize(char *src, int length, char delimiter, char *tokens[], int max_tokens)
{
  int tokens_found = 0;
  char *last_token_pos = src;

  while ((length-- > 0) && (tokens_found < max_tokens))
  {
    if (*src == delimiter)
    {
      *src = '\0';
      tokens[tokens_found++] = last_token_pos;
      last_token_pos = src;
      last_token_pos++;
    }

    src++;
  }

  // save last token which has no delimiter due to end of string
  if (tokens_found < max_tokens)
  {
    tokens[tokens_found++] = last_token_pos;
  }

  return tokens_found;
}
