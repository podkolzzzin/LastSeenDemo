import sys
sys.path.append('../')

import unittest
from unittest.mock import patch, Mock

from fetch_all_data import fetch_all_data

class TestFetchAllData(unittest.TestCase):
    @patch('fetch_all_data.requests.get')
    def test_fetch_all_data_empty_response(self, mock_get):
        # Mock an empty response
        mock_response = Mock()
        mock_response.status_code = 200
        mock_response.json.return_value = {
            "total": 0,
            "data": []
        }
        mock_get.return_value = mock_response

        base_url = "https://example.com/api/users/lastSeen"
        all_data = fetch_all_data(base_url)

        # Assertions
        self.assertEqual(len(all_data), 0)
        mock_get.assert_called_once_with(f"{base_url}?offset=0")

if __name__ == '__main__':
    unittest.main()
