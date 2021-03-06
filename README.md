# Nats-Tools
Some command line tools for Nats: listen, send, request, stats...

Binaries (built by Github/actions): [Win64](https://github.com/fremag/Nats-Tools/suites/794769485/artifacts/8511992) - [Linux](
https://github.com/fremag/Nats-Tools/suites/794769485/artifacts/8511991)

Syntax: nats-tool [command] [options...]

Commands:
````
  send       Send Nats messages
  listen     Listen to Nats subject
  request    Send Nats request messages
  reply      Reply to Nats requests
  stat       Statistics on received Nats messages
  help       Display more information on a specific command.
  version    Display version information.
 ````

# Send
Sends a NATS message.

## Parameters
````
  -s, --subject    Required. Nats message subject
  -m, --msg        Required. Nats message content. Variables: {n} : message number, {time} : local time (format hh:mm:ss.fff)
  -p, --period     (Default: 1000) Time in ms between messages
  -l, --length     (Default: 0) Message length. If -m is specified, its length is filled or reduced as needed. 0 means
                   the message is not modified

  -c, --count      (Default: 1) Exits after n messages
  -w, --wait       (Default: -1) Waits for n seconds then exits
  -n, --nats       Nats server url, default: nats://localhost:4222
  ````

## Examples
* send -s "TEST" -m '{"values":[{    "fruit": "Apple",    "price": "1.23",    "color": "Red"},{    "fruit": "Apple",    "price": "2.34",    "color": "Green"},{    "fruit": "Orange",    "price": "3.45",    "color": "orange"}]}'
````
20:28:07.9603 - Info - nats_tools.SendCommand - Send: A.B.C => 'fruit": "Apple",    "price": "2.34",    "color": "Green"},{    "fruit": "Orange",    "price": "3.45",    "color": "orange"}]}...'
````            

* send -s "A.B.C" -m "Hello ! {n} {time}" -c 10 -p 500
````
20:29:04.6681 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 0 20:29:04.640'
20:29:05.1779 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 1 20:29:05.177'
20:29:05.6789 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 2 20:29:05.678'
20:29:06.1801 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 3 20:29:06.180'
20:29:06.6805 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 4 20:29:06.680'
20:29:07.1811 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 5 20:29:07.181'
20:29:07.6813 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 6 20:29:07.681'
20:29:08.1817 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 7 20:29:08.181'
20:29:08.6825 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 8 20:29:08.682'
20:29:09.1827 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 9 20:29:09.182'
````            
                
* send -s "A.B.C" -m "Hello ! {n} {time}" -w 3 -p 500
````
20:29:54.9600 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 0 20:29:54.952'
20:29:55.4693 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 1 20:29:55.469'
20:29:55.9701 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 2 20:29:55.970'
20:29:56.4709 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 3 20:29:56.470'
20:29:56.9720 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 4 20:29:56.972'
20:29:57.4725 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 5 20:29:57.472'
````            

* send -s "A.B.C" -m "Hello ! {n} {time}" -w 3 -p 500 -l 50
````
20:47:44.6341 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 0 20:47:44.605' - 50 bytes
20:47:45.1438 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 1 20:47:45.143' - 50 bytes
20:47:45.6440 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 2 20:47:45.644' - 50 bytes
20:47:46.1446 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 3 20:47:46.144' - 50 bytes
20:47:46.6456 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 4 20:47:46.645' - 50 bytes
20:47:47.1465 - Info - nats_tools.SendCommand - Send: A.B.C => 'Hello ! 5 20:47:47.146' - 50 bytes
````

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
  -c, --count        Exits after n messages
  -w, --wait         Waits for n seconds then exits
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
# Request
Send a request message and waits the reply
## Parameters
````
  -t, --timeout    (Default: 10000) Time in milliseconds to wait for a reply
  -s, --subject    Required. Nats message subject
  -m, --msg        Required. Nats message content. Variables: {n} : message number, {time} : local time (format
                   hh:mm:ss.fff)
  -p, --period     (Default: 1000) Time in ms between messages
  -l, --length     (Default: 0) Message length. If -m is specified, its length is filled or reduced as needed. 0 means
                   the message is not modified
  -c, --count      (Default: 1) Exits after n messages
  -w, --wait       (Default: -1) Waits for n seconds then exits
  -n, --nats       Nats server url, default: nats://localhost:4222

````

# Reply
Listen to a subject and send a reply message if a request message is received.  

## Parameters
````
  -m, --msg         Required. Nats message content. Variables: {n} : message number, {time} : local time (format
                    hh:mm:ss.fff)
  -l, --length      (Default: 0) Message length. If -m is specified, its length is filled or reduced as needed. 0 means
                    the message is not modified

  -s, --subjects    Nats subjects to listen
  -c, --count       (Default: -1) Exits after n messages
  -w, --wait        (Default: -1) Waits for n seconds then exits
  -n, --nats        Nats server url, default: nats://localhost:4222
````
 
# Stat
Listen to one or more subjects and display some statistics about received messages: count, size etc

## Parameters
````
  -p, --period      (Default: 5000) Time in ms between stats print
  -s, --subjects    Nats subjects to listen
  -c, --count       (Default: -1) Exits after n messages
  -w, --wait        (Default: -1) Waits for n seconds then exits
  -n, --nats        Nats server url, default: nats://localhost:4222
````
