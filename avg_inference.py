def read_log() -> str:
    import os

    path = os.path.join("results", "inference.log")
    with open(path, "r") as f:
        return f.read()


def parse_inference(log: str) -> list[int]:
    import re

    return [int(x) for x in re.findall(r"in (\d+) milliseconds", log)]

def print_stats(values: list[int]) -> None:
    import numpy as np

    print(f"Founded {len(values)} values")
    print(f"Mean: {np.mean(values)}")
    print(f"Median: {np.median(values)}")

    print(f"Percentiles")
    for perc in range(5, 101, 5):
        print(f"  {perc}: {np.percentile(values, perc)}")

if __name__ == "__main__":
    log = read_log()
    values = parse_inference(log)
    print_stats(values)
