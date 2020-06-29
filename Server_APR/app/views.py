"""
Definition of views.
"""

from datetime import datetime
from django.shortcuts import render
from django.http import HttpRequest
from django.http import HttpResponseRedirect

from app.models import RiddleForm, Riddle


def home(request):
    """Renders the home page."""
    assert isinstance(request, HttpRequest)
    query_results = Riddle.objects.all()
    return render(
        request,
        'app/index.html',{
        'query_results': query_results
       }

    )

#def riddle(request):
 #   """Renders the home page."""
  #  assert isinstance(request, HttpRequest)
   # return render(
    #    request,
     #   'app/riddle.html',
      #  {
       #     'title':'Riddle Page',
        #    'year':datetime.now().year,
        #}
   # )

def get_riddle(request):
    # if this is a POST request we need to process the form data
    if request.method == 'POST':
        # create a form instance and populate it with data from the request:
        form = RiddleForm(request.POST)
        # check whether it's valid:
        if form.is_valid():
            # process the data in form.cleaned_data as required
            # ...
            # redirect to a new URL:
            form.save()
            return HttpResponseRedirect('/')

    # if a GET (or any other method) we'll create a blank form
    else:
        form = RiddleForm()

    return render(request, 'app/riddle.html', {'form': form})



