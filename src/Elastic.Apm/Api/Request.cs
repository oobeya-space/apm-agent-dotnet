﻿// Licensed to Elasticsearch B.V under one or more agreements.
// Elasticsearch B.V licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information

using System.Collections.Generic;
using Elastic.Apm.Api.Constraints;
using Elastic.Apm.Helpers;
using Elastic.Apm.Report.Serialization;
using Newtonsoft.Json;

namespace Elastic.Apm.Api
{
	/// <summary>
	/// Encapsulates Request related information that can be attached to an <see cref="ITransaction" /> through
	/// <see cref="ITransaction.Context" />
	/// See <see cref="Context.Request" />
	/// </summary>
	public class Request
	{
		public Request(string method, Url url) => (Method, Url) = (method, url);

		[Sanitize]
		public object Body { get; set; }

		[Sanitize]
		public Dictionary<string, string> Headers { get; set; }

		[JsonProperty("http_version")]
		[MaxLength]
		public string HttpVersion { get; set; }

		[MaxLength]
		[Required]
		public string Method { get; set; }

		public Socket Socket { get; set; }

		[Required]
		public Url Url { get; set; }

		public override string ToString() =>
			new ToStringBuilder(nameof(Request)) { { "Method", Method }, { "Url", Url }, { "Socket", Socket } }.ToString();
	}

	public class Socket
	{
		public bool Encrypted { get; set; }

		[JsonProperty("remote_address")]
		public string RemoteAddress { get; set; }

		public override string ToString() =>
			new ToStringBuilder(nameof(Socket)) { { "Encrypted", Encrypted }, { "RemoteAddress", RemoteAddress } }.ToString();
	}

	public class Url
	{
		private string _full;
		private string _raw;

		[MaxLength]
		public string Full
		{
			get => _full;
			set => _full = Http.Sanitize(value, out var newValue) ? newValue : value;
		}

		[MaxLength]
		[JsonProperty("hostname")]
		public string HostName { get; set; }

		[MaxLength]
		[JsonProperty("pathname")]
		public string PathName { get; set; }

		[MaxLength]
		public string Protocol { get; set; }

		[MaxLength]
		public string Raw
		{
			get => _raw;
			set => _raw = Http.Sanitize(value, out var newValue) ? newValue : value;
		}

		/// <summary>
		/// The search describes the query string of the request.
		/// It is expected to have values delimited by ampersands.
		/// </summary>
		[MaxLength]
		[JsonProperty("search")]
		public string Search { get; set; }

		public override string ToString() => new ToStringBuilder(nameof(Url)) { { "Full", Full } }.ToString();
	}
}
