import os
from pathlib import Path

run_id = "FSMAgent2VectorLightCondition"
embeding_onnx = ["Enemy", "PlayerAgent"]

def get_path_lastest_model(dir: str) -> Path:
    import glob
    from datetime import datetime

    files = []
    path = os.path.join("results", run_id, dir, "*.onnx")
    for file in glob.iglob(path):
        p = Path(file)
        files.append((datetime.fromtimestamp(p.stat().st_mtime), p))
    files.sort(key=lambda x: x[0])
    # pprint(files)
    return files[-1][1]


def embed_weigth(file_path: Path) -> None:
    import onnx

    model_name = file_path.name[: file_path.name.rfind("-")]
    out_path = os.path.join(file_path.parent.parent, model_name + ".onnx")
    model = onnx.load(path, load_external_data=True)
    onnx.save(model, f=out_path, all_tensors_to_one_file=True)


if __name__ == "__main__":
    for onnx in embeding_onnx:
        path = get_path_lastest_model(onnx)
        embed_weigth(path)
