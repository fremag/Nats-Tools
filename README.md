# Nats-Tools
Some command line toosl for Nats: listen, send, request, stats...

Syntax: nats-tool [command] [options...]

Commands:
*  listen     Listen to Nats subject
*  send       Send Nats message
*  help       Display more information on a specific command.
*  version    Display version information.

# Send
Sends a NATS message.

## Parameters
````
  -n, --nats       Nats server url, default: nats://localhost:4222
  -s, --subject    Required. Nats message subject
  -m, --msg        Required. Nats message content
````

## Example
dotnet run  send -s "TEST" -m '{"values":[{    "fruit": "Apple",    "price": "1.23",    "color": "Red"},{    "fruit": "Apple",    "price": "2.34",    "color": "Green"},{    "fruit": "Orange",    "price": "3.45",    "color": "orange"}]}'


# Listen
Listen to one or more subjects, display received messages. 
If message is a JSON string, it's possible to extract tokens from it or format and print it.

## Parameters
````
  -n, --nats         Nats server url, default: nats://localhost:4222
  -s, --subjects     Nats subjects to listen
  -t, --tokens       JSON tokens to extract
  -d, --delimiter    (Default: ,) Token output delimiter
  -j, --json         (Default: false) JSON pretty print
````  

## Examples

Message sent: 
````
'{"values":[{    "fruit": "Apple",    "price": "1.23",    "color": "Red"},{    "fruit": "Apple",    "price": "2.34",    "color": "Green"},{    "fruit": "Orange",    "price": "3.45",    "color": "orange"}]}'
````

* listen -s TEST
````
18:52:07.2019 - Info - TEST - 204 - '{"values":[{    "fruit": "Apple",    "price": "1.23",    "color": "Red"},{    "fruit": "Apple",    "price": "2.34",    "color": "Green"},{    "fruit": "Orange",    "price": "3.45",    "color": "orange"}]}'
````

* listen -s TEST -t "$.values.[0].color"  "$.values.[1].color" "$.values.[2].color"
````
18:53:00.1645 - Info - TEST - 204 - Red,Green,orange
````

* listen -s TEST -j

````
18:54:10.0974 - Info - TEST - 204 - {
  "values": [
    {
      "fruit": "Apple",
      "price": "1.23",
      "color": "Red"
    },
    {
      "fruit": "Apple",
      "price": "2.34",
      "color": "Green"
    },
    {
      "fruit": "Orange",
      "price": "3.45",
      "color": "orange"
    }
  ]
}
````
