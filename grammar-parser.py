import re


log_line = "2021-01-01 12:00:00,123 INFO Some log message"
regex = r"(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2},\d{3}) (\w+) (.*)"


result = re.match(regex, log_line)
if result:
  timestamp = result.group(1)
  log_level = result.group(2)
  message = result.group(3)


  print("Timestamp:", timestamp)
  print("Log level:", log_level)
  print("Message:", message)
else:
  print("Строка лога не соответствует формату")