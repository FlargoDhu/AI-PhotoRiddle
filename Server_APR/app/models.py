"""
Definition of models.
"""

from django.db import models
from django.forms import ModelForm


class Riddle(models.Model):
    Riddle_text = models.CharField("Text of Riddle",max_length=300)
    Riddle_type = models.CharField("Type of Riddle",max_length=30)
    Riddle_dif = models.IntegerField(1)

class RiddleForm(ModelForm):
    class Meta:
        model = Riddle
        fields = ['Riddle_text', 'Riddle_type','Riddle_dif']

class RiddleUsers(models.Model):
    Score = models.IntegerField("Score")

# Create your models here.
