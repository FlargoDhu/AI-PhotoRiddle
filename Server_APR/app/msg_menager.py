from app.models import RiddleUsers,Riddle
from django.core import serializers

def register():
    new_user = RiddleUsers(Score = 0)
    new_user.save()
    return new_user.id

def gen_riddle():
    riddle = Riddle.objects.order_by("?").first()
    return riddle.Riddle_text + "/" + riddle.Riddle_type + "/" + str(riddle.Riddle_dif) + "/" + str(riddle.id)

def update_scores(payload, user_id):
    split_scores = payload.split('/')

    user = RiddleUsers.objects.get(pk = int(user_id))
    user.Score = user.Score + int(split_scores[0])
    user.save()

    riddle = Riddle.objects.get(pk = int(split_scores[1]))
    riddle.Riddle_dif = riddle.Riddle_dif + int(split_scores[2])
    riddle.save()
    return

def gen_leader(user_id):
    userin = False
    users_ten = RiddleUsers.objects.order_by("-Score")[:10]
    response = ""
    for user in users_ten:
        response += str(user.id) + "/" + str(user.Score) + "."
        if(int(user_id) == user.id):
            userin = True
    if(userin == False):
        user = RiddleUsers.objects.get(pk = int(user_id))
        response += str(user.id) + "/" + str(user.Score) + "."
    return response

def respond(payload, order, user_id):
    payload = payload.decode("utf-8")
    if (order == "GEN_RIDDLE"):
        return "APR/"+str(user_id)+"/"+"RIDDLE_RES", gen_riddle()

    if (order == "UPDATE_SCORES"):
        update_scores(payload, user_id)
        return None

    if (order == "GET_L"):
        return "APR/"+str(user_id)+"/RES_L", gen_leader(user_id)

def msg_entrance(payload,topic):

    split_topic = topic.split('/')

    if(split_topic[0]=="APR"):
        if(split_topic[1]!="REGISTER"):
            user_id = int(split_topic[1])
            response = respond(payload, split_topic[2], user_id)
            if(response != None):
                return response[0],response[1]
            else: 
                return "Error"
        else:
            return "APR/REGISTER_RES",register()
    else:
        return "Error"