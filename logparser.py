from logparser import LogParser


log_data = """
2023-12-16 18:30:45 [INFO] - Пользователь вошел в систему
2023-12-16 18:31:20 [ERROR] - Недопустимая операция
2023-12-16 18:32:05 [DEBUG] - Попытка доступа к защищенным данным
"""


parser = LogParser()
parsed_logs = parser.parse_lines(log_data.split('\n'))


for log in parsed_logs:
  print("Время:", log['timestamp'])
  print("Уровень:", log['level'])
  print("Сообщение:", log['message'])