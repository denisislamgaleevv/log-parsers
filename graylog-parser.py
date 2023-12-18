import logging
from graypy import GELFUDPHandler




logger = logging.getLogger('graylog_logger')
logger.setLevel(logging.INFO)
handler = GELFUDPHandler('graylog_server', 12201)
logger.addHandler(handler)




def parse_logs(logs):
  for log in logs:
        parsed_log = f"Parsed: {log}"  
        logger.info(parsed_log)


        logger.info(log)
logs = [
  "Сообщение 1",
  "Сообщение 2",
  "Сообщение 3"
]




parse_logs(logs)