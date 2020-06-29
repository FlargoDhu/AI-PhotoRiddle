import paho.mqtt.client as mqtt
import paho.mqtt.publish as publish
import app.msg_menager as msg_menager

def on_connect(client, userdata, flags, rc):
    client.subscribe("APR/#",0)

def on_message(client, userdata, msg):
    msg_to_publish = msg_menager.msg_entrance(msg.payload, msg.topic)
    if(msg_to_publish != "Error"):
        publish.single(msg_to_publish[0],msg_to_publish[1])



client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message

client.connect("localhost", 1883)
client.loop_start()

