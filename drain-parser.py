import re


def parse_logs(logs):
  parsed_logs = []


  for log in logs:
      match = re.match(r'(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}) - (\w+): (.*)', log)


      if match:
          timestamp = match.group(1)
          class_name = match.group(2)
          message = match.group(3)


          parsed_logs.append({
              'timestamp': timestamp,
              'class': class_name,
              'message': message
          })


  return parsed_logs


logs = [
  '2021-09-30 10:00:00 - ClassA: Error occurred',
  '2021-09-30 11:00:00 - ClassB: Warning: incomplete input',
  '2021-09-30 12:00:00 - ClassA: Status: OK'
]
parsed_logs = parse_logs(logs)
for log in parsed_logs:
  print(f'Timestamp: {log["timestamp"]}, Class: {log["class"]}, Message: {log["message"]}')