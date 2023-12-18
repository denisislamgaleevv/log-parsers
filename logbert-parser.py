from logbert import LogBERTParser


log_data = """
[2023-12-16 18:30:45] INFO: User login successful
[2023-12-16 18:31:20] ERROR: Invalid operation detected
[2023-12-16 18:32:05] DEBUG: Attempt to access sensitive data
"""
parser = LogBERTParser()
parsed_logs = parser.parse(log_data)


for log in parsed_logs:
  if log.level == "INFO":
      print(f"Info log at {log.timestamp}: {log.message}")
  elif log.level == "ERROR":
      print(f"Error log at {log.timestamp}: {log.message}")
  elif log.level == "DEBUG":
      print(f"Debug log at {log.timestamp}: {log.message}")
  else:
      print(f"Unknown log level at {log.timestamp}: {log.message}")
