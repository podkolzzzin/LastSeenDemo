# pip install pytest pytest-subprocess pytest-cov expecter


import unittest
from pytest_subprocess import Subprocess
from expecter import expect


class TestFormatTime(unittest.TestCase):
    def test_When_NowEqualsLastSeen_Expect_Online(fp):
        result = Subprocess.run(['dotnet', 'LastSeenDemo.dll'], stdout=Subprocess.PIPE, stderr=Subprocess.PIPE)
        assert result.returncode == 0
        expect(result.stdout).contains("Expected output")

if __name__ == '__main__':
    unittest.main()
