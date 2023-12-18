import re


def parse_logs(logs, grok_pattern):
  pattern = re.compile(grok_pattern)
  parsed_logs = []
  for log in logs:
      match = pattern.match(log)
      if match:
          parsed_logs.append(match.groupdict())
  return parsed_logs


logs = [
  "INFO: Connection established from 192.168.0.1",
  "ERROR: Invalid username or password",
  "WARNING: Disk space is running low"
]
grok_pattern = r"(?P<severity>\w+): (?P<message>.+)"


parsed_logs = parse_logs(logs, grok_pattern)


for log in parsed_logs:
  print(log['severity'], log['message'])
