/**
 * @file HMI.hpp
 *
 * @date 11.12.2015
 * @author Andre
 * @description
 */
#ifndef HMI_HPP_
#define HMI_HPP_

#include "SystemState.hpp"
#include "SystemCommand.hpp"

class HMI
{
public:
  HMI() { }
  virtual ~HMI() { }

  /** This is the do-work function of the submodule.
   *
   * @param systemState The actual state of the system
   * @param systemCommand Command request from the module
   * @return Return true, if systemCommand contains a valid request;
   *         return false, if no command shall be executed
   */
  virtual bool execute(SystemState& systemState, SystemCommand& systemCommand) = 0;
};

#endif /* HMI_HPP_ */
