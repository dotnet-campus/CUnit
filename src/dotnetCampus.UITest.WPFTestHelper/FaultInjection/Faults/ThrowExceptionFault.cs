// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System;
using dotnetCampus.UITest.WPFTestHelper.FaultInjection.Constants;

namespace dotnetCampus.UITest.WPFTestHelper.FaultInjection.Faults
{
    [Serializable()]
    internal sealed class ThrowExceptionFault : IFault
    {
        public ThrowExceptionFault(Exception exceptionValue)
        {
            if (exceptionValue == null)
            {
                throw new FaultInjectionException(ApiErrorMessages.ExceptionNull);
            }
            this.exceptionValue = exceptionValue;
        }
        public void Retrieve(IRuntimeContext rtx, out Exception exceptionValue, out object returnValue)
        {
            returnValue = null;
            exceptionValue = this.exceptionValue;
        }
        private readonly Exception exceptionValue;
    }
}
