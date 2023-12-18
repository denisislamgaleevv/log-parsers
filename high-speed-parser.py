class LogParser:
  def __init__(self):
      self.parsed_logs = []


  def parse_log(self, log_line):
      log_parts = log_line.split(' ')
      timestamp = log_parts[0]
      level = log_parts[1][1:-1] 
      message = ' '.join(log_parts[2:])
      self.parsed_logs.append({'timestamp': timestamp, 'level': level, 'message': message})


  def get_parsed_logs(self):
      return self.parsed_logs




from multiprocessing import Pool
from typing import List




def parse_logs_parallel(log_data):
  parser = LogParser()
  with Pool(processes=4) as pool:  # Используем 4 процесса
      pool.map(parser.parse_log, log_data)
  return parser.get_parsed_logs()




def main():
  buffer_size = 8192  
  with open('logs.log', 'r', buffering=buffer_size) as file:
      log_data = file.readlines()
 
  parsed_logs = parse_logs_parallel(log_data)


  for log in parsed_logs:
      print(f"Timestamp: {log['timestamp']}, Level: {log['level']}, Message: {log['message']}")

if __name__ == "__main__":
   main()
