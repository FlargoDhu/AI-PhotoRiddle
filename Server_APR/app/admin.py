from django.contrib import admin

from .models import Riddle, RiddleUsers
import app.mqtt as mqtt
admin.site.register(Riddle)
admin.site.register(RiddleUsers)
