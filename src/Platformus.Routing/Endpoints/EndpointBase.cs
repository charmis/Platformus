﻿// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Platformus.Barebone;
using Platformus.Routing.Data.Entities;

namespace Platformus.Routing.Endpoints
{
  public abstract class EndpointBase : IEndpoint
  {
    private Endpoint endpoint;
    private Dictionary<string, string> parameterValuesByCodes;

    public virtual IEnumerable<EndpointParameterGroup> ParameterGroups => new EndpointParameterGroup[] { };
    public virtual string Description => null;

    public IActionResult Invoke(IRequestHandler requestHandler, Endpoint endpoint, IEnumerable<KeyValuePair<string, string>> arguments)
    {
      this.endpoint = endpoint;
      return this.GetActionResult(requestHandler, endpoint, arguments);
    }

    protected abstract IActionResult GetActionResult(IRequestHandler requestHandler, Endpoint endpoint, IEnumerable<KeyValuePair<string, string>> arguments);

    protected bool HasParameter(string key)
    {
      this.CacheParameterValuesByCodes();
      return this.parameterValuesByCodes.ContainsKey(key);
    }

    protected int GetIntParameterValue(string key)
    {
      this.CacheParameterValuesByCodes();

      if (int.TryParse(this.parameterValuesByCodes[key], out int result))
        return result;

      return 0;
    }

    protected bool GetBoolParameterValue(string key)
    {
      this.CacheParameterValuesByCodes();

      if (bool.TryParse(this.parameterValuesByCodes[key], out bool result))
        return result;

      return false;
    }

    protected string GetStringParameterValue(string key)
    {
      this.CacheParameterValuesByCodes();
      return this.parameterValuesByCodes[key];
    }

    private void CacheParameterValuesByCodes()
    {
      if (this.parameterValuesByCodes == null)
        this.parameterValuesByCodes = ParametersParser.Parse(this.endpoint.Parameters).ToDictionary(a => a.Key, a => a.Value);
    }
  }
}