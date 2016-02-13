/**
 * @file tokenize.h
 *
 * @date 10.12.2015
 * @author Andre
 * @description
 */
#ifndef TOKENIZE_H_
#define TOKENIZE_H_

#ifdef __cplusplus
 extern "C" {
#endif

int tokenize(char *src, int length, char delimiter, char *tokens[], int max_tokens);

#ifdef __cplusplus
}
#endif

#endif /* TOKENIZE_H_ */
