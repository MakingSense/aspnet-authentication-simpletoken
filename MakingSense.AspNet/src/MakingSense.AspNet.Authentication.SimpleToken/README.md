# SimpleToken Authentication Middleware

This package allows to extract token from authenticated requests and delegate it to an appropriated ISecurityTokenValidator and generate and AuthenticationTicket.

## Behavior

### Token extraction details

This middleware tries to support almost [RFC 6750](http://tools.ietf.org/html/rfc6750) and some licenses based on [GitHub behavior](https://developer.github.com/v3/oauth/#use-the-access-token-to-access-the-api). But does not support Form-Encoded Body Parameter (http://tools.ietf.org/html/rfc6750#section-2.2).

There are three methods of sending tokens:

* **Authorization Request Header Field**. 

    See [RFC6750 Section 2.1](https://tools.ietf.org/html/rfc6750#section-2.1)

    For example:

	```
    GET /resource HTTP/1.1
    Host: server.example.com
    Authorization: Bearer mF_9.B5f-4.1JqM
	```

	It accepts `Bearer` schema name, but also `OAuth2` and `Token`.

* **URI Query Parameter**

    See [RFC6750 Section 2.1](https://tools.ietf.org/html/rfc6750#section-2.3)

	For example `https://server.example.com/resource?access_token=mF_9.B5f-4.1JqM&p=q`:

	```
	GET /resource?access_token=mF_9.B5f-4.1JqM HTTP/1.1
    Host: server.example.com
	```

* **Basic Authentication with any username and token**

    See [GitHub Basic Authentication Via OAuth Tokens](https://developer.github.com/v3/auth/#via-oauth-tokens)

    For example:

	```
	$ curl -u user:317F632427BCDA059B19EF241705BD2F https://server.example.com/resource
	```

	Or 

	```
    GET /resource
	Host: server.example.com
    Authorization: basic dXNlcjozMTdGNjMyNDI3QkNEQTA1OUIxOUVGMjQxNzA1QkQyRg==
	```

### The WWW-Authenticate Response Header Field

When a protected resource is requested but request does not include authentication credentials or does not contain an access token that enables access it includes the HTTP "WWW-Authenticate" response header field.

For example:

* Request:

    ```
    GET /resource
    Host: server.example.com
	```

* Response    
    
    ```
    Status Code: 401 Unauthorized
    Content-Length: 436
    Content-Type: application/json; charset=utf-8
    Date: Fri, 11 Sep 2015 16:41:50 GMT
    WWW-Authenticate: Bearer
    ```

## Usage 

It is necessary to register all valid `ISecurityTokenValidator` classes and add the middleware to ApplicationBuilder using `UseSimpleTokenAuthentication`.

Example:

```csharp
public class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddMvc();
		services.AddTransient<ISecurityTokenValidator, MyCustomTokenValidator>();
	}

    public void Configure(IApplicationBuilder app)
	{
		app.UseSimpleTokenAuthentication(o =>
		{
			o.AutomaticAuthentication = true;
		});
		app.UseMvc();
	}
}
```

Internally, when the token is not valid or there are not any registered any capable `ISecurityTokenValidator`, an `AuthenticationException` is thrown.