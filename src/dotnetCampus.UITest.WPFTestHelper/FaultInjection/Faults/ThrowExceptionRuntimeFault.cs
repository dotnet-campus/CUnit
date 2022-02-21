// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System;
using dotnetCampus.UITest.WPFTestHelper.FaultInjection.SignatureParsing;

namespace dotnetCampus.UITest.WPFTestHelper.FaultInjection.Faults
{
    [Serializable(), RuntimeFault]
    internal sealed class ThrowExceptionRuntimeFault : IFault
    {
        public ThrowExceptionRuntimeFault(string exceptionExpression)
        {
            this.exceptionExpression = exceptionExpression;
        }
        public void Retrieve(IRuntimeContext rtx, out Exception exceptionValue, out object returnValue)
        {
            returnValue = null;
            exceptionValue = (Exception)Expression.GeneralExpression(exceptionExpression);
        }
        private readonly string exceptionExpression;
    }
}
