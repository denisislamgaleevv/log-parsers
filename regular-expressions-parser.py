import re


class LogParser:
  def __init__(self, log_format):
      self.log_format = log_format
      self.pattern = re.compile(rf"\[(.*?)\] (\w+): (.*)")


  def parse_log(self, log_line):
      match = self.pattern.match(log_line)
      if match:
          timestamp = match.group(1)
          level = match.group(2)
          message = match.group(3)
          return {'timestamp': timestamp, 'level': level, 'message': message}
      else:
          return None


  def parse_logs(self, file_path):
      parsed_logs = []
      with open(file_path, 'r') as file:
          for line in file:
              parsed_log = self.parse_log(line)
              if parsed_log:
                  parsed_logs.append(parsed_log)
      return parsed_logs




log_parser = LogParser(r"\[(.*?)\] (\w+): (.*)")
parsed_logs = log_parser.parse_logs("example.log")
for log in parsed_logs:
  print(log)
